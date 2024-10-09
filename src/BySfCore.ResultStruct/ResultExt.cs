
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BySfCore.ResultStruct;

public static class ResultExt
{
  public static Result ToCustomResultObject(this ModelStateDictionary modelState) {
    var errors = modelState
                 .Where(x => x.Value?.Errors?.Any() == true)
                 .SelectMany(x => x.Value!.Errors.Select(y => new {
                   ParamName = x.Key,
                   Error = y.ErrorMessage
                 }))
                 .ToDictionary(x => x.ParamName, x => x.Error);

    return Result.ValidationErrors(errors);
  }
}