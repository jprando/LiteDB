﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace LiteDB.Shell.Commands
{
    internal class Spool : ConsoleCommand
    {
        private TextWriter _writer;

        public override bool IsCommand(StringScanner s)
        {
            return s.Scan(@"spo(ol)?\s*").Length > 0;
        }

        public override void Execute(ref LiteDatabase db, StringScanner s, Display display, InputCommand input)
        {
            if(s.Scan("false|off").Length > 0 && _writer != null)
            {
                display.TextWriters.Remove(_writer);
                input.OnWrite = null;
                _writer.Flush();
                _writer.Dispose();
                _writer = null;
            }
            else if(_writer == null)
            {
                if (db == null) throw LiteException.NoDatabase();

                var path = Path.GetFullPath(string.Format("LiteDB-spool-{0:yyyy-MM-dd-HH-mm}.txt", DateTime.Now));

                _writer = File.CreateText(path);

                display.TextWriters.Add(_writer);

                input.OnWrite = (t) => _writer.Write(t);
            }
        }
    }
}
