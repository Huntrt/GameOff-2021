# Controls:
	Rightclick - set click enemy as rival
	Re Rightclick - click enemy are no longer rival
	Hold n Drag Rightclick - draw an selection box to set enemy as rival
	X Key - deselect all allies
	Arrow key - Move camera
	Mouse edge - drag the camera toward mouse when mouse of the edge of screen 

# Content:
	Wood ants - attack by spraying it own formic acid
	Fire ants - attack dealing alot of damage combine with dot.

# Details:
	An section has an size of 13x13 0.4 scale path nodes
	Manager.i.path.isScanning = a* scan graph state
	Buying allies in eggs:
		> control get the [eggs]
		> follower go toward the [eggs]
		> follower call the [interact event] of the [eggs] when in range
		> [eggs] will show [eggs panel] and update the panel info upon [interact event]
		> [eggs] will listen to accecpt and decline BUTTON event of [eggs panel]
		a. when click ACCEPT button in [eggs panel]
			> spawning the [eggs]'s allies if has enough money
		b. when click DECLINE button in [eggs panel] or MOVING formation
			> close the panel and remove both the accecpt and decline event in [eggs]