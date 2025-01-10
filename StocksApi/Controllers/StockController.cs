using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksApi.DTOs;
using StocksApi.Errors;
using StocksApi.Extensions;
using StocksApi.IO.Requests.Stock;
using StocksApi.Stocks;

namespace StocksApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
	private readonly StocksService _stocksService;

	public StockController(StocksService stocksService)
	{
		_stocksService = stocksService;
	}

	[HttpGet("[action]")]
	public async Task<IActionResult> GetPrice([FromQuery] string symbol)
	{
		var result = await _stocksService.GetCurrentPrice(symbol);
		if (result.IsFailed)
			return BadRequest(result.GetFirstError().Message);

		return Ok(result.Value);
	}

	[Authorize]
	[HttpPost("[action]/{symbol}")]
	public async Task<IActionResult> HistoricalData([FromBody] HistoricalDataRequest request, string symbol)
	{
		HistoricalDataDto requestDto = new(request.StartDate, request.EndDate);

		var result = await _stocksService.GetHistoricalData(symbol, requestDto);
		if (result.IsFailed)
		{
			return result.GetFirstError() switch
			{
				ValidationError err => ValidationProblem(ModelState.AddValidationErrors(err.Metadata)),
				HistoricalSearchError err => NotFound(err.Message),
			};
		}

		return Ok(result.Value);
	}
}