/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler.chrome_devtools.Types.DOM
{
    public class Node
    {
        [JsonProperty(PropertyName = "nodeId")]
        public int NodeId { get; set; }
        [JsonProperty(PropertyName = "parentId")]
        public int ParentId { get; set; }
        [JsonProperty(PropertyName = "backendNodeId")]
        public int BackendNodeId { get; set; }
        [JsonProperty(PropertyName = "nodeType")]
        public int NodeType { get; set; }
        [JsonProperty(PropertyName = "nodeName")]
        public string NodeName { get; set; }
        [JsonProperty(PropertyName = "localName")]
        public string LocalName { get; set; }
        [JsonProperty(PropertyName = "nodeValue")]
        public string NodeValue { get; set; }
        [JsonProperty(PropertyName = "childNodeCount")]
        public int ChildNodeCount { get; set; }
        [JsonProperty(PropertyName = "children")]
        public Node[] Children { get; set; }
        [JsonProperty(PropertyName = "attributes")]
        public string[] Attributes { get; set; }
        [JsonProperty(PropertyName = "documentURL")]
        public string DocumentURL { get; set; }
        [JsonProperty(PropertyName = "baseURL")]
        public string BaseURL { get; set; }
        [JsonProperty(PropertyName = "publicId")]
        public string PublicId { get; set; }
        [JsonProperty(PropertyName = "systemId")]
        public string SystemId { get; set; }
        [JsonProperty(PropertyName = "internalSubset")]
        public string InternalSubset { get; set; }
        [JsonProperty(PropertyName = "xmlVersion")]
        public string XmlVersion { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "pseudoType")]
        public string PseudoType { get; set; }
        [JsonProperty(PropertyName = "shadowRootType")]
        public string ShadowRootType { get; set; }
        [JsonProperty(PropertyName = "frameId")]
        public string FrameId { get; set; }
        [JsonProperty(PropertyName = "contentDocument")]
        public Node ContentDocument { get; set; }
        [JsonProperty(PropertyName = "shadowRoots")]
        public Node[] ShadowRoots { get; set; }
        [JsonProperty(PropertyName = "templateContent")]
        public Node TemplateContent { get; set; }
        [JsonProperty(PropertyName = "pseudoElements")]
        public Node[] PseudoElements { get; set; }
        [JsonProperty(PropertyName = "importedDocument")]
        public Node ImportedDocument { get; set; }
        [JsonProperty(PropertyName = "distributedNodes")]
        public BackendNode[] DistributedNodes { get; set; }
        [JsonProperty(PropertyName = "isSVG")]
        public bool IsSVG { get; set; }
    }
}
