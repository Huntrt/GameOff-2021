# Controls:
	Rightclick - set click enemy as rival
	Re Rightclick - click enemy are no longer rival
	Hold n Drag Rightclick - draw an selection box to set enemy as rival
	X Key - deselect all allies
	
	Function:
	[Formator.cs]
		> Has the [followers], [rivals] and [selectings] list
		> An follower can get order in [followers] list
		> Add all the [selectings] enemy as [rivals]
		> Clear all the currently [selectings] enemy
		> Add, remove an enemy as rival by click on that enemy
		> Formator has an [target rival] function that will update when select rival
		> Upon [target rival] it will set the all follower target to be rival equally
	[Controls.cs]
		> Sending the an click enemy to [rivals]
		> Draw an [selector box] when drag while holding mouse
		> [selector box] will selecting all the enemy inside it
		> Release mouse while there is [selector box] will rival all the [selectings] enemy
		> Press an key will remove all follower's rival, target and it move path
	[GoalsCreate]
		> Generated an grid of goal base on [followers] amount
	[Followers.cs]
		> On start
			> Add itself to [followers] list
			> Get it own order at [Formator.cs]
		> An function that able to set this follower [target destination]
		> [follower state] will be change to chase when move toward an rival
		> [follower state] will be change to fight in range of an rival
	[Allies.cs]
		> Handle attack when [follower state] are now fight