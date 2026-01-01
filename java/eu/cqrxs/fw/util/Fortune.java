/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2027 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

package eu.cqrxs.fw.util;


import java.io.Serializable;
import java.lang.Exception;
import java.lang.String;
import java.util.Random;

/**
 * Fortune is a never ready fortunes shuffle
 */
public class Fortune {

	static String[] fortunes = {
		"You have not to bear an ounce of guilt.\n\n\t-- Major Tom (came from acorn, passed dos / windows, went to linux / aix)\n", 
		"Word Perfect is a good word processor.  It just doesn't run on any os.\n",
		"When Alan Cox came to Auditorium Maximum at TU Vienna,\nthe entire Auditorium was plain full.\n\n\t-mArch\n",
		"Without GNU tar I couldn't survive 2 minutes on Solaris, HPUX and Irix.\n\n\t-- Martin from gs (ghostscript)\n",
		"Mike is an excellent programmer and scientist, but always sells himself under market value,\nbut everyone else is happy then, because some money is still there for risk buffer of others.\n\n\t-- Mr. president Wolfman\n",
		"FW_ALLOW_SOURCE_SQUENCH: ... otherwise you will be open to a DDOS attack, so choose your poison.\n\t-- SuSE kernel\n",
		"I don't know, what will happen, if we fork() a next step application.\n\t-- Thomas B.\n",
		"\tI have the right to choose my OS in lab, I said:\n\t\t\"I don't want Mikey Mouse DOS, I choose linux,\n\t\tbut that couldn't damage anything.\n\t\tNext day DOS will be still intact here again.\"\n\n\tAssistant Prof. didn't believe me,\n\tbut at midnight DOS was reinstalled\n\tper dhcp, bootp, tftp from the server.\n\n\ttom@logic\n\n(I saw it with my own eyes)\n",
		"\tGod created the albino\npigementation fault (core dump)\n\n\t-- me myself (zen@area23.at)\n",
		"AT&T believed the world is a 32bit VAX little endian.\n\n\t-- mike@rainbow\n",
		"Software is like sex, if it's free it's better.\n\n\t-- Linus Torvalds\n",
		"Try `stty 0' -- it works much better.\n",
		"Trap full -- please empty.\n",
		"Tomorrow's computers some time next month.\n\n\t-- DEC\n",
		"To communicate is the beginning of understanding.\n\n\t-- AT&T\n",
		"Those who can't write, write manuals.\n",
		"Those who can't LaTex, write manuals in word\n",
		"This file will self-destruct in five minutes.\n",
		"* * * * * THIS TERMINAL IS IN USE * * * * *\n",
        "I'm a Lisp variable -- bind me!\n",
        "Be warned that typing \fBkillall \fIname\fP may not have the desired\neffect on non-Linux systems, especially when done by a privileged user.\n\t-- From the killall manual page\n"
	};
	
	public static String[] getFortunes() {
			return fortunes;
	}
	
	public static String getFortune() {
		Random rand = new Random();		
		int r = ((r = rand.nextInt())< 0) ? ((0 - r)%fortunes.length) : (r%fortunes.length);
		return fortunes[r];
	}
}
