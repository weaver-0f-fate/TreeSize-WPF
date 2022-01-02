﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Models {
    public class DirectoryFile : AbstractFile {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryFile(DirectoryInfo info) : base(info) {
            _directoryInfo = info;
            IsDirectory = true;
            NestedItems = new ObservableCollection<AbstractFile>();
            BindingOperations.EnableCollectionSynchronization(NestedItems, new object());
        }

        public void LoadNestedDirectories() {
            foreach (var dir in _directoryInfo.GetDirectories()) {
                try {
                    NestedItems.Add(new DirectoryFile(dir));
                }
                catch (Exception) { }
            }
        }

        public void LoadNestedFiles() {
            foreach (var file in _directoryInfo.GetFiles()) {
                NestedItems.Add(new SystemFile(file));
                Size += file.Length;
            }
        }
    }
}