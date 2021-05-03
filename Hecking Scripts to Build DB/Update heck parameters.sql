

select * from  `heckdata`.`heckparameter`;
UPDATE `heckdata`.`heckparameter` SET Param_Value = 'Hello! I am friend heck bot. My friends call me heck bot. 
 You may speak to me using the following commands:
 &heck: Use this command, then specify a user, a value (Not required. Defaults to 1. User must have hecks available to heck with.), and a reason for their hecking.
	NOTE: You can also reply to a user with "heck u" to instantly heck them for the message they sent.
 &info: Use this command to see your heck total and available hecks. You may also mention a user specifically to see their heck total.
 &leaderboard: Allows you to see the current users in the server ranked by heck-ness.
 &rec: View the last 5 hecks received by the user (@ someone to see their hecks or don''t @ to see your''s).
 &sent: View the last 5 hecks sent by the user (@ someone to see their hecks or don''t @ to see your''s).
 &version: Display current heck version.
 &roadmap: See what is planned for the FUTURE. @.@ 
 &who: I guess I can tell you what''s up. Use this command to find out what the heck is going on. @.@ 
 &help: Displays this message. Heck you.' WHERE PARAM_NAME = 'HELP';
 
 