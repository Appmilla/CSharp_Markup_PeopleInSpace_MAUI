using Refit;

namespace CSharpMarkupPeopleInSpaceMaui.Apis;

public interface ISpaceXApi
{        
    [Get("/crew")]
    Task<string> GetAllCrew();
}