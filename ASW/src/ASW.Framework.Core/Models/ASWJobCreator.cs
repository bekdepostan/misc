using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public abstract class ASWJobCreator<T> where T : ASWJob
    {
        protected ASWJobCreator<T> _nextRouter = null;

        public ASWJobCreator(ASWJobCreator<T> nextRouter)
        {
            _nextRouter = nextRouter;
        }

        protected abstract bool Compatible(ApplicationCase appCase, T currentJob);

        protected abstract T CreateJob(ApplicationCase appCase, T currentJob);

        public T GetNextJob(ApplicationCase appCase, T currentJob)
        {
            if (Compatible(appCase, currentJob))
            {
                return CreateJob(appCase, currentJob);
            }
            else if (_nextRouter != null)
            {
                return _nextRouter.GetNextJob(appCase, currentJob);
            }
            else
            {
                return null;
            }
        }
    }
}
