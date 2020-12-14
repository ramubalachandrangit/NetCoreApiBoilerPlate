using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Service
{
    public partial class Service: IService
    {
        private readonly IInstitutionRepository _institutionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IWordStoreRepository _wordStoreRepository;
        private readonly ILogger _logger;
        public Service()
        {

        }

        public Service(ILogger<Service> logger, IWordStoreRepository wordStoreRepository, IStudentRepository studentRepository, IInstitutionRepository institutionRepository)
        {
            _logger = logger;
            _wordStoreRepository = wordStoreRepository;
            _studentRepository = studentRepository;
            _institutionRepository = institutionRepository;
        }
    }
}
