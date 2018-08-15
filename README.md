Compile using Visual Studio-15.8+ or `msbuild /restore` and configure IIS Express to see this site. You might place something like this under `<sites/>` in `applicationhost.config`:

```xml
<site name="StreamStreamedDuringHeadRequest" id="3">
    <application path="/">
        <virtualDirectory path="/" physicalPath="%USERPROFILE%\source\repos\StreamStreamedDuringHeadRequest\StreamStreamedDuringHeadRequest" />
    </application>
    <bindings>
        <binding protocol="http" bindingInformation=":8080:localhost" />
    </bindings>
</site>
```

Then start:

```
iisexpress /Site:StreamStreamedDuringHeadRequest
```

In another terminal, run a normal `GET` request. You should see that the whole stream is spooled by watching the output of IISExpress and also that the content is properly downloaded to your HTTP client:

```
curl -si http://localhost:8080/api/Default
```

IISExpress output (duplicated `Request ended` is a separate issue reported in https://github.com/aspnet/AspNetWebStack/issues/184):

```
Request started: "GET" http://localhost:8080/api/Default
Read(, 0, 4096)
 =1080
Request ended: http://localhost:8080/api/Default with HTTP status 200.0
Read(, 0, 4096)
 =0
Dispose(True)
Request ended: http://localhost:8080/api/Default with HTTP status 200.0
```

Now, run a `HEAD` request. You will see from the IISExpress output that the entire stream is streamed needlessly even though no content is sent to the HTTP client:

```
curl -sI http://localhost:8080/api/Default
```

IISExpress output:

```
Request started: "HEAD" http://localhost:8080/api/Default
Read(, 0, 4096)
 =1080
Request ended: http://localhost:8080/api/Default with HTTP status 200.0
Read(, 0, 4096)
 =0
Dispose(True)
Request ended: http://localhost:8080/api/Default with HTTP status 200.0
```

(To verify that no content is sent to the client, you can run the following command which causes `curl` to wait for content. If the server behaves correctly and does **not** send content, then **the following command should hang** after it gets the headers until the connection is terminated by the server or the client is killed. `curl` will also correctly warn you that `-XHEAD` is unlikely to perform correctly):

```
curl -siXHEAD http://localhost:8080/api/Default
```
