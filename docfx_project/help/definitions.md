# Definitions

Below are definitions for several terms used in throughout this documentation.

### Member
A Member represents a person in the organization. Member objects store identifying information about the people they represent. Members can be assigned a Position within a Component. See the section on [Creating a Member](create_member.md) for a detailed description of the Member object.

### Position
A Position represents a job or role within a specific Component in the organization, such as an Administrative Aide, Director, or Commander. A Component can have several Positions to which Members may be assigned. See the section on [Creating a Position](create_position.md) for a detailed description of the Position object.

### Component
A Component represents a subdivision of the organization, such as a Bureau, Division, District, Section, Squad, and many others. A Component is the core element that BlueDeck uses to represent the organization's hierarchical structure. When a Component is created, it is assigned as a *child* of another Component, which is the the *Parent.* A Component can have unlimited children, but only a single parent. This relationship ensures that BlueDeck can accurately render and parse the structure of the organization. See the section on [Creating a Component](create_component.md) for a detailed description of the Component object.

