# Log4OMQSLEmailerPNG

You need an image to use as a base template (you can have more than 1) 

Added DataLayout Form to select font size and location to overlay text for qsl data.  

Added logging so we can avoid duplicates if the data in the log does not get updated correct - or more importantly so we an use ADIF files down the road.  

Currently hard coded Arial font - regular - Black. 

Exclusion list should be comma seperated call signs.   ei ,AC9HP,KB9BVN,WB9FU, 

Not that it starts with a comma and ends with a comma.   Commas are used to make sure the call sign is an exact match and not partial. 

