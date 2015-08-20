using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    class DeepHierarchyEditableModel : EditableModel
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
        public IEnumerable<CompositeEditableModel> CompositeModels
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
    }
}
