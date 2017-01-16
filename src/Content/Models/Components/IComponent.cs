using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Content.Models.Components
{
    public interface IComponent
    {
        string ComponentType { get; }
        Guid Id { get; }
    }

    public abstract class BaseComponent : IComponent
    {
        public IDictionary<string, ComponentSettings> Children { get; private set; } = new Dictionary<string, ComponentSettings>();
                
        public string ComponentType { get; }
        public Guid Id { get; }

        public BaseComponent(string name)
        {
            ComponentType = name;
            Id = Guid.NewGuid();
        }

        public void AddChild(string name, IComponent component, bool CanHaveMultipleItems = false)
        {
            Children.Add(name, new ComponentSettings()
            {
                CanHaveMultipleItems = CanHaveMultipleItems,
                Component = component
            });
        }
    }

    public class PrimitiveComponent : IComponent
    {
        private static IDictionary<PrimitiveType, Guid> PRIMITIVE_IDS = new Dictionary<PrimitiveType, Guid>()
        {
            { PrimitiveType.String, new Guid("1bb9b671-b2a4-4a0b-9546-f08be506387e") },
            { PrimitiveType.Boolean, new Guid("6cef2eb5-6be7-4d8c-9422-4e7b3230bc85") },
            { PrimitiveType.Integer, new Guid("62c687e3-6fb4-45b5-83f3-0601854393ee") },
        };

        public string ComponentType { get; }

        public Guid Id {get; }

        private PrimitiveComponent(PrimitiveType type)
        {
            ComponentType = type.ToString();
            Id = PRIMITIVE_IDS[type];
        }
        
        public static implicit operator PrimitiveComponent(PrimitiveType type)
        {
            return new PrimitiveComponent(type);
        }
    }

    public class ContainerComponent : BaseComponent
    {
        public ContainerComponent(string name) : base(name)
        {

        }
    }

    public enum PrimitiveType
    {
        @String,
        @Boolean,
        @Integer
    }

    public class ComponentSettings
    {        
        public bool CanHaveMultipleItems { get; set; }

        public IComponent Component { get; set; }        
    }

    public class ComponentRepository
    {
        private IDictionary<Guid, IComponent> _components = new Dictionary<Guid, IComponent>();

        public IReadOnlyDictionary<Guid, IComponent> Components { get
            {
                return new ReadOnlyDictionary<Guid, IComponent>(_components);
            } }


        public void AddComponent(IComponent component)
        {
            var baseComponent = component as BaseComponent;
            
            if (baseComponent != null)
            {
                foreach (var child in baseComponent.Children)
                {
                    AddComponent(child.Value.Component);
                }
            }

            if (_components.ContainsKey(component.Id))
                return;

            _components.Add(component.Id, component);
        }
    }
}
