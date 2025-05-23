﻿using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> 
        (ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request={Request} - {Response} - RequestData={RequestData}", 
                typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = new Stopwatch();
            timer.Start();

            //Continua la Execucio del Pipeline
            var response = await next();
            //Quan acaba Torna a parar el timer
            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)
            {
                logger.LogWarning("[PERFORMANCE] The request={Request} took {TimeTaken}",
                    typeof(TRequest).Name, timeTaken);
            }

            logger.LogInformation("[END] Handle request={Request} - {Response}",
                typeof(TRequest).Name, typeof(TResponse).Name);
            return response;
        }
    }
}
