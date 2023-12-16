// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Jmelosegui.Tools
{
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using Microsoft.Data.SqlClient;

    internal sealed class ConnectCommand : CommandBase
    {
        public ConnectCommand(InvocationContext context)
            : base(context)
        {
        }

        required public string ConnectionString { get; init; }

        internal static Command Create()
        {
            var connectionString = new Option<string>("--connection-string", "The connection string to use when connecting to the server")
            {
                IsRequired = true,
            };

            connectionString.AddAlias("-cs");

            Command command = new Command("connect", "Connect to a Microsoft Sql Server instance")
            {
                connectionString,
            };

            command.SetHandler(ctx => new ConnectCommand(ctx)
            {
                ConnectionString = ctx.ParseResult.GetValueForOption(connectionString)!,
            }.ExecuteAndDisposeAsync());

            return command;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteCoreAsync()
        {
            using (var connection = new SqlConnection(this.ConnectionString))
            {
                await connection.OpenAsync(this.CancellationToken);
                System.Console.ForegroundColor = ConsoleColor.Green;
                this.Console.WriteLine($"Successfully connected to server: {connection.DataSource}");
                System.Console.ResetColor();
            }
        }
    }
}
