using AutoMapper;
using Longbeach_Customer.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Longbeach_Customer.Services;

public class CustomerServices(ICustomerRepository customerRepo, ICustomerHistoryRepository customerHistoryRepo, IUnitOfWork unitOfWork, ILogger<ClientSourceServices> logger, IMapper mapper) : ICustomerServices
{
    public async Task<Results<Ok<HashCodeResponse>, BadRequest>> AddCustomerAsync(HttpContext httpContext, CustomerRequest customerRequest)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(customerRequest.Name))
            {
                logger.LogError("Customer name is empty");
            }

            if(customerRequest.TaxCode != null && customerRequest.TaxCode.Length >= 10)
            {
                var customerByTaxcode = await customerRepo.GetCustomerByTaxcodeAsync(customerRequest.TaxCode);
                if(customerByTaxcode != null)
                {
                    logger.LogError($"Duplicated taxcode {customerRequest.TaxCode}");
                    return TypedResults.BadRequest();
                }
            }

            var customer = mapper.Map<Customer>(customerRequest);
            string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;
            if (string.IsNullOrEmpty(clientSourceCode))
            {
                return TypedResults.BadRequest();
            }

            customer.SetAdditionalProperties(clientSourceCode);

            await customerRepo.AddCustomerAsync(customer);
            unitOfWork.Commit();

            logger.LogInformation($"Customer id: {customer.Id} added successfully");
            return TypedResults.Ok(new HashCodeResponse { HashCode = customer.HashCode, Id = customer.Id});
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError($"Failed to add customer: {ex.Message}");
            return TypedResults.BadRequest();
        }
    }

    public async Task<Results<Ok, BadRequest>> DeleteCustomerAsync(HttpContext httpContext, Guid id)
    {
        var customerInDb = await customerRepo.GetCustomerByIdAsync(id);
        if (customerInDb == null)
        {
            logger.LogError($"Customer with id {id} not found.");
            return TypedResults.BadRequest();
        }

        string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;
        if (string.IsNullOrEmpty(clientSourceCode))
        {
            return TypedResults.BadRequest();
        }

        var customerHistory = mapper.Map<CustomerHistory>(customerInDb);
        customerHistory.SetAdditionalProperties(clientSourceCode, CustomerHistoryAddedByAction.Deleted);
        try
        {
            await customerHistoryRepo.AddCustomerHistoryAsync(customerHistory);
            await customerRepo.DeleteCustomerAsync(id);
            unitOfWork.Commit();
            logger.LogInformation($"Customer with id {id} deleted successfully.");
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError($"Failed to delete customer: {ex.Message}");
            return TypedResults.BadRequest();
        }
    }

    public async Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await customerRepo.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            logger.LogWarning($"Customer with id {id} not found.");
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Retrieved customer with id {id} successfully.");
        var customerResponse = mapper.Map<CustomerResponse>(customer);
        return TypedResults.Ok(customerResponse);
    }

    public async Task<Results<Ok<IEnumerable<CustomerResponse>>, NotFound>> GetCustomerByPhoneAsync(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            logger.LogWarning("Phone number is null or empty.");
            return TypedResults.NotFound();
        }

        var customer = await customerRepo.GetCustomersByPhoneAsync(phoneNumber);

        if (!customer.Any())
        {
            logger.LogWarning($"Customer with phone {phoneNumber} not found.");
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Retrieved customer with phone {phoneNumber} successfully.");
        var customerResponse = mapper.Map<IEnumerable<CustomerResponse>>(customer);

        return TypedResults.Ok(customerResponse);
    }

    public async Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerByTaxcodeAsync(string taxcode)
    {
        if (string.IsNullOrWhiteSpace(taxcode))
        {
            logger.LogWarning("Taxcode is null or empty.");
            return TypedResults.NotFound();
        }

        var customer = await customerRepo.GetCustomerByTaxcodeAsync(taxcode);

        if (customer == null)
        {
            logger.LogWarning($"Customer with taxcode {taxcode} not found.");
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Retrieved customer with id {taxcode} successfully.");
        var customerResponse = mapper.Map<CustomerResponse>(customer);
        return TypedResults.Ok(customerResponse);
    }

    public async Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerByPearlCustomerCodeAsync(string pearlCustomerCode)
    {
        if (string.IsNullOrWhiteSpace(pearlCustomerCode))
        {
            logger.LogWarning("Pearl customer code is null or empty.");
            return TypedResults.NotFound();
        }

        var customer = await customerRepo.GetCustomerByPearlCustomerCodeAsync(pearlCustomerCode);

        if (customer == null)
        {
            logger.LogWarning($"Customer with pearlCustomerCode {pearlCustomerCode} not found.");
            return TypedResults.NotFound();
        }

        logger.LogInformation($"Retrieved customer with pearlCustomerCode {pearlCustomerCode} successfully.");
        var customerResponse = mapper.Map<CustomerResponse>(customer);
        return TypedResults.Ok(customerResponse);
    }

    public async Task<Results<Ok, NotFound>> IsNewVersionCustomerAsync(Guid id, string hashCode)
    {
        var result = await customerRepo.IsNewVersionCustomerAsync(id, hashCode);

        if (!result)
        {
            logger.LogWarning($"Customer with id {id} is not a new version.");
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }

    public async Task<Results<Ok<HashCodeResponse>, BadRequest>> UpdateCustomerAsync(HttpContext httpContext, CustomerRequest customerRequest, Guid id)
    {
        var customerInDb = await customerRepo.GetCustomerByIdAsync(id);
        if (customerInDb == null)
        {
            logger.LogError($"Customer with id {id} not found.");
            return TypedResults.BadRequest();
        }

        if (customerInDb.HashCode != customerRequest.HashCode)
        {
            logger.LogError($"Hash code mismatch for customer with id {id}");
            return TypedResults.BadRequest();
        }

        try
        {
            var customer = mapper.Map<Customer>(customerRequest);
            customer.Id = id; // Ensure the ID is set for the update

            var customerHistory = mapper.Map<CustomerHistory>(customerInDb);

            string clientSourceCode = httpContext.User.FindFirst("ClientSourceCode")!.Value;
            if (string.IsNullOrEmpty(clientSourceCode))
            {
                return TypedResults.BadRequest();
            }

            customer.SetUpdateProperties(clientSourceCode);
            customerHistory.SetAdditionalProperties(clientSourceCode, CustomerHistoryAddedByAction.Updated);

            await customerRepo.UpdateCustomerAsync(customer);
            await customerHistoryRepo.AddCustomerHistoryAsync(customerHistory);
            unitOfWork.Commit();

            logger.LogInformation($"Customer with id {id} updated successfully.");
            return TypedResults.Ok(new HashCodeResponse { HashCode = customer.HashCode, Id = customer.Id });
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();
            logger.LogError($"Failed to update customer: {ex.Message}");
            return TypedResults.BadRequest();
        }
    }
}
