# BrushlingsCodeSnippits
This repo is a sampling of the scrtipts that I worked on for Brushlings Pale Moon.

**Please do not edit or make use of these scripts without asking for permission as I have spent a lot of time on them and would like to preserve what I have done.**

I imported my capstone BaseEnemy.cs script to Brushlings Pale Moon with a key difference I did not have to make the basic enemies utilize nav Points they were free to roam and BaseEnemyScript.cs was born. FireBoss.cs is an example of using inheritance for the method functionalities that I needed in order to make the FireBoss.cs work.

I also used a type of singleton pattern called GlobalVariables.cs and it made use of the static proterty so. I set up the FireBoss.cs and FireBossManager.cs using the GlobalVariables.cs singleton pattern by going into the FireBoss.cs, for example, and in the awake function I put GlobalVariables.FireBoss = this; so that I could use the script without having to use the get component function or find object of type function which i found can be pretty performance heavy.
