﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-26
// Last Modified:			2016-07-02
// 

using cloudscribe.Logging.Common.Models;
using NoDb;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Logging.NoDb
{
    public class LogItemStoragePathResolver : IStoragePathResolver<LogItem>
    {
        public LogItemStoragePathResolver(
            IStoragePathOptionsResolver storageOptionsResolver)
        {
            optionsResolver = storageOptionsResolver;
        }

        private IStoragePathOptionsResolver optionsResolver;

        public async Task<string> ResolvePath(
            string projectId,
            string key = "",
            string fileExtension = ".json",
            bool ensureFoldersExist = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");

            var pathOptions = await optionsResolver.Resolve(projectId).ConfigureAwait(false);

            var firstFolderPath = pathOptions.AppRootFolderPath
                + pathOptions.BaseFolderVPath.Replace("/", pathOptions.FolderSeparator);

            if (ensureFoldersExist && !Directory.Exists(firstFolderPath))
            {
                Directory.CreateDirectory(firstFolderPath);
            }

            var projectsFolderPath = Path.Combine(firstFolderPath, pathOptions.ProjectsFolderName);

            if (ensureFoldersExist && !Directory.Exists(projectsFolderPath))
            {
                Directory.CreateDirectory(projectsFolderPath);
            }

            var projectIdFolderPath = Path.Combine(projectsFolderPath, projectId);

            if (ensureFoldersExist && !Directory.Exists(projectIdFolderPath))
            {
                Directory.CreateDirectory(projectIdFolderPath);
            }

            var type = typeof(LogItem).Name.ToLowerInvariant();

            var typeFolderPath = Path.Combine(projectIdFolderPath, type.ToLowerInvariant().Trim());

            if (ensureFoldersExist && !Directory.Exists(typeFolderPath))
            {
                Directory.CreateDirectory(typeFolderPath);
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                return typeFolderPath;
            }

            var fileName = key + fileExtension;
            var filePath = Path.Combine(typeFolderPath, key + fileExtension);
            if (File.Exists(filePath)) return filePath;

            // if the file is not found in the type folder
            // we need to check for deeper folders 
            // if the file is found there then return the path
            foreach (string file in Directory.EnumerateFiles(
                typeFolderPath,
                fileName,
                SearchOption.AllDirectories)
                )
            {
                return file;
            }
            //var typeDir = new DirectoryInfo(typeFolderPath);
            //foreach (var levelDir in typeDir.GetDirectories())
            //{
            //    foreach (var dateDir in levelDir.GetDirectories())
            //    {
            //        var fp = Path.Combine(dateDir.FullName, fileName);
            //        if (File.Exists(fp)) return fp;
            //    }

            //}

            // otherwise return the best path calculation based on info provided
            // ie to save a file the other ResolvePath method should be used and the
            // LogEvent should be passed in to determine the path
            return filePath;

        }

        /// <summary>
        /// this method should be used to calculate the path to create or update a LogEvent
        /// it will use the Level and timestamp in determining the path
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="key"></param>
        /// <param name="logEvent"></param>
        /// <param name="fileExtension"></param>
        /// <param name="ensureFoldersExist"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> ResolvePath(
            string projectId,
            string key,
            LogItem logItem,
            string fileExtension = ".json",
            bool ensureFoldersExist = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");

            var pathOptions = await optionsResolver.Resolve(projectId).ConfigureAwait(false);

            var firstFolderPath = pathOptions.AppRootFolderPath
                + pathOptions.BaseFolderVPath.Replace("/", pathOptions.FolderSeparator);

            if (ensureFoldersExist && !Directory.Exists(firstFolderPath))
            {
                Directory.CreateDirectory(firstFolderPath);
            }

            var projectsFolderPath = Path.Combine(firstFolderPath, pathOptions.ProjectsFolderName);

            if (ensureFoldersExist && !Directory.Exists(projectsFolderPath))
            {
                Directory.CreateDirectory(projectsFolderPath);
            }

            var projectIdFolderPath = Path.Combine(projectsFolderPath, projectId);

            if (ensureFoldersExist && !Directory.Exists(projectIdFolderPath))
            {
                Directory.CreateDirectory(projectIdFolderPath);
            }

            var type = typeof(LogItem).Name.ToLowerInvariant();

            var typeFolderPath = Path.Combine(projectIdFolderPath, type.ToLowerInvariant().Trim());

            if (ensureFoldersExist && !Directory.Exists(typeFolderPath))
            {
                Directory.CreateDirectory(typeFolderPath);
            }

            var levelPath = Path.Combine(typeFolderPath, logItem.LogLevel.ToString());

            if (ensureFoldersExist && !Directory.Exists(levelPath))
            {
                Directory.CreateDirectory(levelPath);
            }
;
            var dateFolderName = logItem.LogDateUtc.ToString("yyyyMMdd");

            var datePath = Path.Combine(levelPath, dateFolderName);

            if (ensureFoldersExist && !Directory.Exists(datePath))
            {
                Directory.CreateDirectory(datePath);
            }

            // we don't care if this file exists
            // this method is for calculating where to save 
            return Path.Combine(datePath, key + fileExtension);
        }

    }
}
