
namespace EventSys {
	public struct Event_Plane_FlyIn {
	}

	public struct Event_Plane_OnTarget {
		public StoryMapPoint Point;
	}

	public struct Event_Plane_Hidden {
	}

	public struct Event_NewEventCreated {
		public string EventId;
		public StoryMapPoint Point;
	}

	public struct Event_PlayerActionMade {
		public string EventId;
		public StoryMapPoint Point;
		public StoryEvent.ResultOptions.ResultType ActionType;
	}

	public struct Event_StoryPointDone {
		public string EventId;
		public StoryMapPoint Point;
		public StoryEvent.ResultOptions.ResultType ActionType;
	}



}
