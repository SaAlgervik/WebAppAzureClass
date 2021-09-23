using System.Threading.Tasks;

public interface ICosmosDbService
{
    Task AddAsync(User item);
}


