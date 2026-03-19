using System.Threading.Tasks;

namespace TH1.Patterns.Command
{
    public interface ICommand<T>
    {
        Task<T> ExecuteAsync();
    }
}