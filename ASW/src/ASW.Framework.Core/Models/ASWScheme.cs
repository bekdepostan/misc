using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASW.Framework.Core
{
    public abstract class ASWScheme<T> where T : ASWJob
    {
        public ASWScheme(Application application)
        {
            _application = application;
            _jobCreator = InitJobCreator();
        }

        protected Application _application;
        protected ASWJobCreator<T> _jobCreator;
        protected abstract ASWJobCreator<T> InitJobCreator();
        public T GetNextJob()
        {
            return GetNextJob(null, JobResults.None);
        }
        public T GetNextJob(string currentJobCode, JobResults currentJobResult, NameValueCollection data = null)
        {
            if (_jobCreator != null)
            {
                return _jobCreator.GetNextJob(_application, currentJobCode, currentJobResult, data);
            }
            else
            {
                return null;
            }
        }
    }
}
