/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2027 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

package eu.cqrxs.util;

import java.lang.String;

/**
 * Fortune is a never ready fortunes shuffle
 */
public class Fortune {

	final static java.lang.String aviWidgerson = "Universal Cryptography\n\n The late 1970s and early 1980s marked a groundbreaking shift in cryptography,'changing this ancient art into a science,' in research honored in three separate Turing awards as Whitfield Diffie and Martin Hellman; Ronald Rivest, Adi Shamir and Len Adleman; and Shafi Goldwasser and Silvio Micali established the mathematical foundations of modern cryptography. Their work enabled secure encryption methods, leading to protocols for applications like secure online voting, playing poker over the phone, and calculating an average salary without revealing individual salaries.\n\nThese solutions remained ad hoc, requiring unique designs for each application. In the 1980s, Wigderson took this work to the next level, developing general methods that worked for a broad range of applications. With Oded Goldreich and Silvio Micali, Wigderson showed all verification problems have 'zero-knowledge' proofs. For instance, if Bob is attempting a Sudoku puzzle and Alice knows the solution, Alice can convince Bob with high confidence that a solution exists without revealing any part of it. This technique applies to any NP problem, including the Traveling Salesman and Map Coloring problems. Later researchers built on these ideas to create protocols for proving identity without exposing a secret key—essential for the secure smart chips in our credit cards. The same team also designed the first general-purpose method to play games involving hidden information, such as poker, bridge, or complex auctions, fairly and securely online. This method maintains security even if a small number of cheating players try to collude.\n\nTaming Randomness\n\nLike his work on complexity and cryptography, Wigderson’s contributions to probabilistic computing built upon foundational work of the 1970s.\n\nConsider trying to test whether a given number is prime, a key step for modern cryptographic protocols. You could try all the possible factors but large numbers have too many potential factors to try. In the 1970s, Robert Solovay and Volker Strassen, among others, developed an efficient randomized algorithm for testing primality, and John Gill developed a full complexity theory for probabilistic computation.\n\n"  +
   "In practice computers don’t flip coins, they create fake randomness by looking at outputs of a complicated function. These pseudorandom generators were ad hoc without a strong theoretical foundation. In the late 1980s, Wigderson and his colleagues discovered a formal method for creating pseudorandom generators from computationally hard functions. Wigderson, in a series of papers co-authored with Noam Nisan, Russell Impagliazzo and others, showed how to use suitably hard functions to create strong pseudorandom generators whose outputs are computationally indistinguishable from true random bits. The outputs of these generators can replace the randomness in any probabilistic algorithm. \n\nAt the turn of the century, Avi Wigderson, by then a professor at the Institute of Advanced Study, continued to make advances in understanding randomness. Imagine trying to connect several cities with a minimal number of roads, so that by taking a random road out of each city, you’d quickly end up in a completely random city. Such networks are called “expander graphs”. Together with Omer Reingold and Salil Vadhan, Wigderson developed a new “zig-zag” construction for creating these graphs—a clever recursive method with an elegantly simple proof of correctness.\n\nThe zig-zag construction led to an algorithm that could tell, using a small amount of memory, whether two cities on a map were connected. Reingold’s algorithm used the zig-zag construction to make the cities close together if they were connected at all. In this new map, you can just look at all short trips from one city to see if it reaches the other.\n\nExpanders also have applications for coding techniques able to automatically correct the errors introduced when bits get garbled in transmission, as often happens over wireless links. Using error-correcting codes a sender expands the message in a specific way that allows the receiver to recover the original message even if a small number of characters have been altered. Stronger bounds on expanders lead to more efficient error-correcting codes.\n\nExpanders serve as a starting point for extracting randomness out of weak sources. Suppose you need truly random bits but you only have access to data on sunspot activity which is variable but far from uniformly random. Through several papers and collaborators, Avi Wigderson devised ways to accomplish this, either by adding a small amount of additional randomness or by using multiple independent weak sources of random bits.\n\n" + 
    "Wigderson’s solutions to problems often have applications to seemingly unrelated questions. Think of a Ramsey graph like a social network where there are no moderately-sized groups who are either all friends with each other, or none of whom are friends with each other. Extractors developed by Wigderson and his successors lead to new constructions of Ramsey Graphs that increased the size of the groups that were not all connected or disconnected.\n\nWigderson’s Legacy\n\nThe research I have discussed here encompass just a small part of Avi Wigderson’s wide-ranging work. He has over a hundred publications in the two major theoretical computer science conferences, the ACM Symposium on the Theory of Computing and the IEEE Symposium on the Foundations of Computer Science (a total much higher than that of any other researcher). Wigderson rarely works alone, with nearly 200 distinct co-authors.\n\nIn 1999, Wigderson joined the Institute of Advanced Study in Princeton as the Herbert H. Maass Professor in the School of Mathematics. Almost immediately, IAS became a center for research in computational complexity as Wigderson hosted visiting students, postdocs and visiting faculty. Many of today’s leaders in the field honed their skills at the institute.\n\nWigderson served on the scientific advisory board of the Simons foundation and his advocacy led to the creation of the Simons Institute for the Theory of Computing in Berkeley, California, itself a major center for the theory community.\n\nIn his 2019 book Mathematics and Computation: A Theory Revolutionizing Technology and Science published by Princeton University Press, Wigderson argued that the P versus NP problem, computational complexity, and theoretical computer science more broadly, together constitute a philosophy that is central not only to computer science but also biology, economics, physics and the social sciences. Nevertheless he has never shied away from defining the field as a serious mathematical discipline. When someone suggested that the implementer of an algorithm get as much credit as the one who discovered it, Wigderson quipped, “Let them try to do it first.\n\nAuthor: Lance Fortnow [https://amturing.acm.org/award_winners/wigderson_3844537.cfm]";

	public final static java.lang.String[] fortunes = {
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
        "Be warned that typing \fBkillall \fIname\fP may not have the desired\neffect on non-Linux systems, especially when done by a privileged user.\n\t-- From the killall manual page\n",
        aviWidgerson,
        "How does RL relate to the psychology of animal behavior?\n\nBroadly speaking, RL works as a pretty good model of instrumental learning, though a detailed argument for this has never been publically made (the closest to this is probably Barto, Sutton and Watkins, 1990). On the other hand, the links between classical (or Pavlovian) conditioning and temporal-difference (TD) learning (one of the central elements of RL) are close and widely acknowledged (see Sutton and Barto, 1990).\n\n Ron Sun has developed hybrid models combining high-level and low-level skill learning, based in part on RL, which make contact with psychological data (see Sun, Merrill, and Peterson, 2001).\nhttp://incompleteideas.net/RL-FAQ.html"

	};
	
	public static java.lang.String[] getFortunes() {
			return fortunes;
	}
	
	public static java.lang.String getFortune() {
		java.util.Random rand = new java.util.Random();
		int r = ((r = rand.nextInt())< 0) ? ((0 - r)%fortunes.length) : (r%fortunes.length);
		return fortunes[r];
	}
}
