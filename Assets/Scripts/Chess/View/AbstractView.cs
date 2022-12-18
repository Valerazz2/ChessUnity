using System.Collections.Generic;
using UnityEngine;

namespace Chess.View
{
    public abstract class AbstractView<TM> : MonoBehaviour
    {
        public TM model { get; private set; }

        protected void Bind(TM _model)
        {
            model = _model;
            OnBind();
        }

        protected abstract void OnBind();
        
        protected void CreateViews<M, V>(IEnumerable<M> modelList, V viewPrefab) where V : AbstractView<M>
        {
            foreach (var model in modelList)
            {
                CreateView(model, viewPrefab);
            }
        }
        
        protected void CreateView<M, V>(M model, V viewPrefab) where V : AbstractView<M>
        {
            var view = Instantiate(viewPrefab);
            view.Bind(model);
        }

        protected virtual void UnBind()
        {
            model = default(TM);
        }
        
    }
}
