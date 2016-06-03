using System.Threading.Tasks;
using Orleans;
using System.Collections.Generic;
using System;

namespace ASW.Framework.Core
{
    /// <summary>
    /// Grain interface IGrain1
    /// </summary>
	public interface ITeam : IGrainWithGuidKey
    {
        Task<Guid> GetId();

        // team has a name
        Task<string> GetName();
        Task SetName(string name);

        Task SignIn(IASWCenter center, bool localMode);
        Task SignOff();
    }
}
