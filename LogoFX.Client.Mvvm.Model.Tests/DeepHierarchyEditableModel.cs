using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LogoFX.Client.Mvvm.Model.Tests
{
    class DeepHierarchyEditableModel : EditableModel
    {        

        public DeepHierarchyEditableModel(IEnumerable<CompositeEditableModel> compositeModels)
        {
            foreach (var compositeModel in compositeModels)
            {
                _compositeModels.Add(compositeModel);
            }
        }

        private readonly ObservableCollection<CompositeEditableModel> _compositeModels = new ObservableCollection<CompositeEditableModel>();

        public IEnumerable<CompositeEditableModel> CompositeModels
        {
            get { return _compositeModels;}
        }

        public void RemoveCompositeItem(CompositeEditableModel item)
        {
            MakeDirty();
            _compositeModels.Remove(item);
        }
    }
}
