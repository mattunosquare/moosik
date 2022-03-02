﻿using System.Diagnostics.CodeAnalysis;

namespace moosik.api.ViewModels.Errors;
[ExcludeFromCodeCoverage]
public class BadRequestViewModel
{
    public string type { get; set; }
    public string title { get; set; }
    public int status { get; set; }
    public string traceId { get; set; }
    public Dictionary<string,string[]> errors { get; set; }
}