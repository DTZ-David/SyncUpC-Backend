using SyncUpC.Domain.Ports;
using SyncUpC.Domain.Ports.Configuration.Claims;
using SyncUpC.Domain.Ports.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Infraestructure.Adapters;

public class UnitOfWork : IUnitOfWork
{
    public IAccountService AccountService { get; }

    public IClaimService ClaimsService { get; }

    public IStudentService StudentService {  get; }

    public UnitOfWork(IAccountService accountService,
                      IStudentService studentService,
                      IClaimService claimsService)
    {
        AccountService = accountService;
        StudentService = studentService;
        ClaimsService = claimsService;
    }

}
