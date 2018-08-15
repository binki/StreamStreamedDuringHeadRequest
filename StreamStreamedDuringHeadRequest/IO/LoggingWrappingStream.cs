using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStreamedDuringHeadRequest.IO
{
    class LoggingWrappingStream : Stream
    {
        readonly Stream stream;

        public override bool CanRead => stream.CanRead;

        public override bool CanSeek => stream.CanSeek;

        public override bool CanWrite => stream.CanWrite;

        public override long Length => stream.Length;

        public override long Position { get => stream.Position; set => Seek(value, SeekOrigin.Begin); }

        public LoggingWrappingStream(
            Stream stream)
        {
            this.stream = stream;
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            Console.WriteLine($"CopyToAsync({destination}, {bufferSize}, {cancellationToken}");
            return stream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            Console.WriteLine($"Dispose({disposing})");
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            Console.WriteLine($"Flush()");
            stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Console.WriteLine($"Read(, {offset}, {count})");
            var result = stream.Read(buffer, offset, count);
            Console.WriteLine($" ={result}");
            return result;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            Console.WriteLine($"ReadAsync({buffer}, {offset}, {count}, {cancellationToken})");
            var result = await stream.ReadAsync(buffer, offset, count, cancellationToken);
            Console.WriteLine($" ={result}");
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            Console.WriteLine($"Seek({offset}, {origin})");
            var result = stream.Seek(offset, origin);
            Console.WriteLine($" ={result}");
            return result;
        }

        public override void SetLength(long value)
        {
            Console.WriteLine($"SetLength({value})");
            stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Console.WriteLine($"Write({buffer}, {offset}, {count})");
            stream.Write(buffer, offset, count);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            Console.WriteLine($"WriteAsync({buffer}, {offset}, {count}, {cancellationToken})");
            return base.WriteAsync(buffer, offset, count, cancellationToken);
        }
    }
}