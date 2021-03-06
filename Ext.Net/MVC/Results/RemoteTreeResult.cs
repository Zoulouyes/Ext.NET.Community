/********
 * This file is part of Ext.NET.
 *     
 * Ext.NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU AFFERO GENERAL PUBLIC LICENSE as 
 * published by the Free Software Foundation, either version 3 of the 
 * License, or (at your option) any later version.
 * 
 * Ext.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU AFFERO GENERAL PUBLIC LICENSE for more details.
 * 
 * You should have received a copy of the GNU AFFERO GENERAL PUBLIC LICENSE
 * along with Ext.NET.  If not, see <http://www.gnu.org/licenses/>.
 *
 *
 * @version   : 2.1.1 - Ext.NET Community License (AGPLv3 License)
 * @author    : Ext.NET, Inc. http://www.ext.net/
 * @date      : 2012-12-10
 * @copyright : Copyright (c) 2007-2012, Ext.NET, Inc. (http://www.ext.net/). All rights reserved.
 * @license   : GNU AFFERO GENERAL PUBLIC LICENSE (AGPL) 3.0. 
 *              See license.txt and http://www.ext.net/license/.
 *              See AGPL License at http://www.gnu.org/licenses/agpl-3.0.txt
 ********/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Ext.Net.Utilities;

namespace Ext.Net.MVC
{
    public class RemoteTreeResult : ActionResult
    {
        private bool accept = true;

        public bool Accept
        {
            get
            {
                return this.accept;
            }
            set
            {
                this.accept = value;
            }
        }

        public string RefusalMessage
        {
            get;
            set;
        }

        public object Attributes
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

        public string NodeID
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            JObject result = new JObject();
            JObject response = new JObject();

            result["actionSuccess"] = this.Accept;
            
            if (this.RefusalMessage.IsNotEmpty())
            {
                result["message"] = this.RefusalMessage;
            }

            if (this.Attributes != null)
            {
                response["attributes"] = new JRaw(JSON.Serialize(this.Attributes));
            }

            if (this.Value != null)
            {
                if (this.Value is string) 
                {
                    response["value"] = this.Value.ToString();
                }
                else
                {
                    response["value"] = new JRaw(JSON.Serialize(this.Value));
                }
                
            }

            if (this.NodeID != null)
            {
                response["id"] = this.NodeID;
            }

            if (response.Count > 0)
            {
                result["response"] = response;
            }

            CompressionUtils.GZipAndSend(result.ToString(Newtonsoft.Json.Formatting.None));
        }
    }
}