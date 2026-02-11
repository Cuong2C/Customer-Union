namespace Customer_Union.EndpointHandlers.ClientSourceHandlers
{
    public class GetAllClientSourceHandler(IGetAllClientSource getAllClientSource, IMapper mapper)
    {
        public async Task<Ok<IEnumerable<ClientSourceResponse>>> GetAllClientSourcesAsync()
        {
            var clientSources = await getAllClientSource.GetAllClientSourcesAsync();
            var result = mapper.Map<IEnumerable<ClientSourceResponse>>(clientSources);

            return TypedResults.Ok(result);
        }
    }
}
