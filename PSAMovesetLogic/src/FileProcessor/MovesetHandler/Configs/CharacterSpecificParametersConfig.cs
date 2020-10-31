using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.Configs
{
    public class CharacterSpecificParametersConfig
    {
        public string DataSize { get; set; }
        public List<Parameter> Parameters { get; set; }
        public List<Article> Articles { get; set; }
        public List<ExtraParameter> ExtraParameters { get; set; }
    }

    public class Parameter
    {
        public string Location { get; set; }
        public string ParameterIndex { get; set; }
        public string NumberOfParameters { get; set; }
        public string Name { get; set; }
        public List<Value> Values { get; set; }
    }

    public class Article
    {
        public string Location { get; set; }
        public string ArticleIndex { get; set; }
        public int NumberOfActions { get; set; }
        public int NumberOfSubActions { get; set; }
        public string Name { get; set; }
        public List<ArticleParameter> ArticleParameters { get; set; }
    }

    public class ArticleParameter
    {
        public string Location { get; set; }
        public string ParameterIndex { get; set; }
        public string NumberOfParameters { get; set; }
        public string Name { get; set; }
        public List<Value> Values { get; set; }
    }

    public class ExtraParameter
    {
        public string Location { get; set; }
        public string ParameterIndex { get; set; }
        public List<string> ParameterLocationPath { get; set; }
        public string Name { get; set; }
        public List<Value> Values { get; set; }
    }

    public class Value
    {
        public string Location { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
