# Function:
	[Formator.cs]
		> Has the [followers], [rivals] and [selectings] list
		> An follower can get order in [followers] list
		> Add all the [selectings] enemy as [rivals]
		> Clear all the currently [selectings] enemy
		> Add, remove an enemy as rival by click on that enemy
	[Command.cs]
		> Sending the an click enemy to [rivals]
		> Draw an [selector box] when drag while holding mouse
		> [selector box] will selecting all the enemy inside it
		> Release mouse while there is [selector box] will rival all the [selectings] enemy
	[GOalsCreate]
		> Generated an grid of goal base on [followers] amount
	[Followers.cs]
		> On start
			> Add itself to [followers] list
			> Get it own order at [Formator.cs]
# Control:
	Rightclick - set click enemy as rival
	Re Rightclick - click enemy are no longer rival
	Hold n Drag Rightclick - draw an selection box to set enemy as rival
	X Key - deselect all allies