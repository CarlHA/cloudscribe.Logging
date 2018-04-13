﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-07-02
// Last Modified:			2016-07-02
//

using cloudscribe.Logging.Common.Models;
using System.Collections.Generic;

namespace cloudscribe.Logging.Web.Models
{
    public class PagedQueryResult
    {
        public PagedQueryResult()
        {
            Items = new List<ILogItem>();
        }

        public List<ILogItem> Items { get; set; }

        public int TotalItems { get; set; } = 0;

    }
}
