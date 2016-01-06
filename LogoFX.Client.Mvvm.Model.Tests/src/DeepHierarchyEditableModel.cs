using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    internal interface IDeepHierarchyEditableModel : IEditableModel
    {
        IEnumerable<ICompositeEditableModel> CompositeModels { get; }
    }

    class DeepHierarchyEditableModel : EditableModel, IDeepHierarchyEditableModel
    {        
        public DeepHierarchyEditableModel(IEnumerable<CompositeEditableModel> compositeModels)
            :this()
        {
            foreach (var compositeModel in compositeModels)
            {
                _compositeModels.Add(compositeModel);
            }
        }

        public DeepHierarchyEditableModel()
        {

        }

        private readonly ObservableCollection<CompositeEditableModel> _compositeModels = new ObservableCollection<CompositeEditableModel>();        

        [EditableList]
        public IEnumerable<ICompositeEditableModel> CompositeModels
        {
            get { return _compositeModels;}
        }

        public void AddCompositeItem(CompositeEditableModel item)
        {
            MakeDirty();
            AddCompositeItemImpl(item);
        }

        internal void AddCompositeItemImpl(CompositeEditableModel item)
        {
            _compositeModels.Add(item);
        }

        public void RemoveCompositeModel(CompositeEditableModel compositeModel)
        {
            MakeDirty();
            _compositeModels.Remove(compositeModel);
        }
    }    
}
