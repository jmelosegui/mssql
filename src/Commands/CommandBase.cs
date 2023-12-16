// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Jmelosegui.Tools
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;

    internal abstract class CommandBase : IDisposable
    {
        protected CommandBase(InvocationContext context)
        {
            this.InvocationContext = context;
            this.Console = context.Console;
            this.CancellationToken = context.GetCancellationToken();
        }

        public int ExitCode { get; set; }

        public CancellationToken CancellationToken { get; }

        public IConsole Console { get; } = new TestConsole();

        protected InvocationContext? InvocationContext { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task ExecuteAndDisposeAsync()
        {
            await this.ExecuteCoreAsync();
            this.Dispose();
        }

        protected abstract Task ExecuteCoreAsync();

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
