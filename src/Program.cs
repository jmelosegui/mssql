// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Jmelosegui.Tools;
using Microsoft.Data.SqlClient;

RootCommand rootCommand = new RootCommand("A cli tool to interact with Microsoft Sql Server")
{
    ConnectCommand.Create(),
};

rootCommand.Name = "mssql";

return await new CommandLineBuilder(rootCommand)
    .UseDefaults()
    .UseExceptionHandler(ExceptionHandler)
    .Build()
    .InvokeAsync(args);

static void ExceptionHandler(Exception ex, InvocationContext context)
{
    Console.ForegroundColor = ConsoleColor.Red;

    switch (ex)
    {
        case SqlException slqEx:
            context.ExitCode = slqEx.ErrorCode;
            break;
        default:
            context.ExitCode = 1;
            break;
    }

    context.Console.Error.Write(ex.Message.ToString());
    Console.ResetColor();
}