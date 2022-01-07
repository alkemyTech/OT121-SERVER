using System;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;

namespace OngProject.Common
{
	public class ResultValue<T> : Result
	{
        public T Value { get; set; }
        public int StatusCode { get; set; }

        public ResultValue()
		{
		}
	}
}

