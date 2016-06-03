using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public class Team : Grain, ITeam
    {
        private Guid _id;
        private string _name;
        private IASWCenter _center;

        public Team()
        {

        }

        public Team(Guid id)
        {
            _id = id;
        }

        public override Task OnActivateAsync()
        {
            _id = this.GetPrimaryKey();
            _name = this.GetPrimaryKey().ToString() ;
            return base.OnActivateAsync();
        }

        public Task<string> GetName()
        {
            return Task.FromResult(_name);
        }

        public Task SetName(string name)
        {
            _name = name;
            return TaskDone.Done;
        }

        public Task SignIn(IASWCenter center, bool localMode = false)
        {
            _center = center;
            if (!localMode)
            {
                return center.AddTeam(_id);
            }
            else
            {
                return center.AddTeam(this);
            }
        }

        public Task SignOff()
        {
            return _center.RemoveTeam(_id);
        }

        public override Task OnDeactivateAsync()
        {
            SignOff();
            return base.OnDeactivateAsync();
        }

        public Task<Guid> GetId()
        {
            return Task.FromResult(_id);
        }
    }
}
