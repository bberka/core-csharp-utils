namespace BySfCore.ResultStruct;

public readonly struct Result
{
  public bool Status { get; init; }

  public string[] Errors { get; init; }

  public string Level { get; init; }

  internal Result(bool status, string[] errors, string level, Exception? exceptionInfo = null) {
    Status = status;
    Errors = errors;
    Level = level;
    _exceptionInfo = exceptionInfo;
  }

  private readonly Exception? _exceptionInfo;

  public Exception? GetException() {
    return _exceptionInfo;
  }

  public static Result Success(string message = "Başarılı") {
    return new Result(true, [message], "info");
  }

  public static Result Success(string[] messages) {
    return new Result(true, messages, "info");
  }

  public static Result Warn(string message = "Başarısız") {
    return new Result(false, [message], "warn");
  }

  public static Result Warn(string[] messages) {
    return new Result(false, messages, "warn");
  }

  public static Result Error(string message = "Hata") {
    return new Result(false, [message], "error");
  }

  public static Result Error(string[] messages) {
    return new Result(false, messages, "error");
  }

  public static Result Exception(Exception exception, string message = "Hata") {
    return new Result(false, [message], "error", exception);
  }

  public static Result Exception(Exception exception, string[] messages) {
    return new Result(false, messages, "error", exception);
  }

  public static Result ValidationError(string key, string message) {
    return new Result(false, [key + ResultGlobalSettings.VALIDATION_ERROR_SEPARATION_CHAR + message], "warn");
  }

  public static Result ValidationErrors(Dictionary<string, string> errors) {
    return new Result(false, errors.Select(x => x.Key + ResultGlobalSettings.VALIDATION_ERROR_SEPARATION_CHAR + x.Value).ToArray(), "warn");
  }

  public static implicit operator bool(Result result) {
    return result.Status;
  }
}

public readonly struct ResultData<T>
{
  public bool Status { get; init; }
  public string[] Errors { get; init; }
  public string Level { get; init; }
  public T? Data { get; init; }

  internal ResultData(bool status, string[] errors, string level, T? data) {
    Status = status;
    Errors = errors;
    Level = level;
    Data = data;
  }

  private Exception? ExceptionInfo { get; init; }

  public Exception? GetException() {
    return ExceptionInfo;
  }

  public static ResultData<T> Success(T data, string message = "Başarılı") {
    return new ResultData<T>(true, [message], "info", data);
  }

  public static ResultData<T> Success(T data, string[] messages) {
    return new ResultData<T>(true, messages, "info", data);
  }

  public static ResultData<T> Warn(string message = "Başarısız", T? data = default) {
    return new ResultData<T>(false, [message], "warn", data);
  }

  public static ResultData<T> Warn(string[] messages, T? data = default) {
    return new ResultData<T>(false, messages, "warn", data);
  }

  public static ResultData<T> Error(string message = "Hata", T? data = default) {
    return new ResultData<T>(false, [message], "error", data);
  }

  public static ResultData<T> Error(string[] messages, T? data = default) {
    return new ResultData<T>(false, messages, "error", data);
  }

  public static ResultData<T> Exception(Exception exception, string message = "Hata", T? data = default) {
    return new ResultData<T>(false, [message], "error", data) { ExceptionInfo = exception };
  }

  public static ResultData<T> Exception(Exception exception, string[] messages, T? data = default) {
    return new ResultData<T>(false, messages, "error", data) { ExceptionInfo = exception };
  }

  public static implicit operator ResultData<T>(Result result) {
    return result.Status
             ? throw new Exception("Can not convert success result to result data")
             : new ResultData<T>(false, result.Errors, result.Level, default);
  }

  public static implicit operator Result(ResultData<T> resultData) {
    return new Result(resultData.Status, resultData.Errors, resultData.Level, resultData.ExceptionInfo);
  }

  public static implicit operator ResultData<T>(T data) {
    return data != null
             ? new ResultData<T>(true, [], "info", data)
             : new ResultData<T>(false, ["Bulunamadı"], "error", data);
  }

  public static implicit operator bool(ResultData<T> result) {
    return result.Status;
  }

  public Result ToResult() {
    return new Result(Status, Errors, Level, ExceptionInfo);
  }
}