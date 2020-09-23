using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Service
{
    public partial class Service: IService
    {
        private readonly IWordStoreRepository _wordStoreRepository;
        private readonly ILogger _logger;
        public Service()
        {

        }

        public Service(ILogger<Service> logger, IWordStoreRepository wordStoreRepository)
        {
            _logger = logger;
            _wordStoreRepository = wordStoreRepository;
        }
    }
}
