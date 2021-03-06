namespace CodeContest.Core.Delphi
{
    using System.Diagnostics;
    using System.IO;
    using System;
    using CodeContest.Core.Generics;

    /// <summary>
    /// Delphi code builder.
    /// </summary>
    public class DelphiBuilder : BaseBuilder, IBuilder
    {
        /// <inheritdoc />
        protected override ProcessStartInfo GetBuilderProcessStartInfo(string path, string code, out string binaryPath)
        {
            var file = $@"{path}\Console.dpr";
            File.WriteAllText(file, code);
            binaryPath = $@"{path}\Console.exe";
            return new ProcessStartInfo("dcc32.exe", file);
        }
    }
}
