﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterOfOffer
{
    public class DatabasePath
    {
        public string DbPath { get; set; }

        public DatabasePath()
        {
            string savedPath = Properties.Settings.Default.DatabasePath;

            if (!string.IsNullOrWhiteSpace(savedPath))
            {
                DbPath = savedPath;
            }
            else
            {
                DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LetterOfOffer", "MyDatabase.sqlite");
            }
        }

    }

}
