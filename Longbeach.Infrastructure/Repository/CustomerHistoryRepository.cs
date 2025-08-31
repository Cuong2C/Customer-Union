using Dapper;
using Longbeach.Domain.Entities;
using Longbeach.Domain.IRepository;
using Longbeach.Infrastructure.Data;

namespace Longbeach.Infrastructure.Repository;

public class CustomerHistoryRepository(IUnitOfWork unitOfWork) : ICustomerHistoryRepository
{
    private const string INSERT_CUSTOMERHISTORY_QUERY = @"
        INSERT INTO CustomerHistories (
            Id, ten_kh, ma_so_thue, dia_chi, dien_thoai, dien_thoai2, dien_thoai3, e_mail, quoc_tich, tinh_thanh, quan_huyen, gioi_tinh, ngay_sinh, 
            tk_nh, ten_nh, loai_khach_ban, ma_kh_pearl, CreatedAt, UpdatedAt, CreatedClientSourceCode, UpdatedClientSourceCode, HashCode, ActionTime, AddedByAction, ChangedByClient
        )
        VALUES (
            @Id, @Name, @TaxCode, @Address, @Phone, @Phone2, @Phone3, @Email, @Nationality, @Province, @District, @Gender, @DateOfBirth, 
            @BankAccount, @BankName, @CustomerType, @PearlCustomerCode, @CreatedAt, @UpdatedAt, @CreatedClientSourceCode, @UpdatedClientSourceCode, @HashCode, @ActionTime, @AddedByAction, @ChangedByClient
        )";
    public async Task<int> AddCustomerHistoryAsync(CustomerHistory customerHistory)
    {
        var connection = unitOfWork.Connection;
        return await connection.ExecuteAsync(
            INSERT_CUSTOMERHISTORY_QUERY,
            new 
            {
                customerHistory.Id,
                customerHistory.Name,
                customerHistory.TaxCode,
                customerHistory.Address,
                customerHistory.Phone,
                customerHistory.Phone2,
                customerHistory.Phone3,
                customerHistory.Email,
                customerHistory.Nationality,
                customerHistory.Province,
                customerHistory.District,
                customerHistory.Gender,
                customerHistory.DateOfBirth,
                customerHistory.BankAccount,
                customerHistory.BankName,
                customerHistory.CustomerType,
                customerHistory.PearlCustomerCode,
                customerHistory.CreatedAt,
                customerHistory.UpdatedAt,
                customerHistory.CreatedClientSourceCode,
                customerHistory.UpdatedClientSourceCode,
                customerHistory.HashCode,
                customerHistory.ActionTime,
                customerHistory.AddedByAction,
                customerHistory.ChangedByClient
            }, 
            transaction: unitOfWork.Transaction);

    }
}
