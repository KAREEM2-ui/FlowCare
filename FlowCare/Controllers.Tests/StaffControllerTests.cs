//using FlowCare_presentation.Controllers;
//using FlowCare.Application.Interfaces.Services;
//using FlowCare.Application.DTOs.Responses;
//using FlowCare.Application.Pagination;
//using FlowCare.Domain.Entities;
//using FlowCare.Domain.Enums;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace Controllers.Tests
//{
//    public class StaffControllerTests
//    {
//        private readonly Mock<IStaffService> _staffServiceMock = new();
//        private readonly Mock<IStaffServiceTypeService> _staffServiceTypeServiceMock = new();
//        private readonly Mock<IAuditLogService> _auditLogServiceMock = new();
//        private readonly Mock<IAppAuthorizationService> _appAuthorizationServiceMock = new();
//        private readonly StaffController _controller;

//        public StaffControllerTests()
//        {
//            _controller = new StaffController(
//                _staffServiceMock.Object,
//                _staffServiceTypeServiceMock.Object,
//                _auditLogServiceMock.Object,
//                _appAuthorizationServiceMock.Object);
//        }

//        [Fact]
//        public async Task ListStaff_Admin_ReturnsPagedResponse()
//        {
//            var paged = new PagedResponse<Staff> { Result = new List<Staff>(), Total = 0 };
//            _staffServiceMock.Setup(s => s.GetAllStaffPaginatedAsync(0, 20, null, It.IsAny<CancellationToken>())).ReturnsAsync(paged);
//            var result = await _controller.ListStaff(CancellationToken.None);
//            Assert.IsType<OkObjectResult>(result);
//        }

//        [Fact]
//        public async Task ListStaff_ByBranch_Unauthorized()
//        {
//            _appAuthorizationServiceMock.Setup(s => s.IsAssignedBranch(It.IsAny<System.Security.Claims.ClaimsPrincipal>(), 1)).Returns(false);
//            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _controller.ListStaff(1, CancellationToken.None));
//        }

//        [Fact]
//        public async Task AssignStaffToService_Success()
//        {
//            _staffServiceTypeServiceMock.Setup(s => s.AssignAsync(1, 2)).Returns(Task.CompletedTask);
//            _auditLogServiceMock.Setup(s => s.LogAsync(It.IsAny<AuditLog>())).Returns(Task.CompletedTask);
//            var result = await _controller.AssignStaffToService(1, 2);
//            Assert.IsType<OkObjectResult>(result);
//        }

//        [Fact]
//        public async Task AssignStaffToBranch_Success()
//        {
//            var staff = new Staff();
//            _staffServiceMock.Setup(s => s.ChangeStaffBranchAsync(1, 2)).ReturnsAsync(staff);
//            _auditLogServiceMock.Setup(s => s.LogAsync(It.IsAny<AuditLog>())).Returns(Task.CompletedTask);
//            var result = await _controller.AssignStaffToBranch(1, 2);
//            Assert.IsType<OkObjectResult>(result);
//        }

//        [Fact]
//        public async Task GetStaffServices_ReturnsServices()
//        {
//            _staffServiceTypeServiceMock.Setup(s => s.GetServicesOfStaffByStaffIdAsync(1)).ReturnsAsync(new List<ServiceType>());
//            var result = await _controller.GetStaffServices(1);
//            Assert.IsType<OkObjectResult>(result);
//        }
//    }
//}
