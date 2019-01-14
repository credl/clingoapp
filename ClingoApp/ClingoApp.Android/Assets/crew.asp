% This encoding is based on an encoding by Hakan Kjellerstrand, hakank@gmail.com
% See also http://www.hakank.org/answer_set_programming/

%
% Crew allocation in ASP.
%
% From Gecode example crew
% examples/crew.cc
%
% (Original text from crew.cc)
% """
% Example: Airline crew allocation
%
% Assign x flight attendants to y flights. Each flight needs a certain
% number of cabin crew, and they have to speak certain languages.
% Every cabin crew member has z flights off after an attended flight.
% """

% a flight must have a crew of exactly Num persons
{ crew(Flight, Person) : p(Person, Type) } = Num :- num_crew(Flight, Num).

% the crew must have at least Num persons of a certain type
:- flight(Flight, Type, Num), #count{ Person : crew(Flight, Person), p(Person, Type) } < Num.

% every cabin crew member must have (a least) number of flights off after an attended flight
:- crew(Flight1, Person), crew(Flight2, Person), min_rest(X), Flight1 < Flight2, Flight2 <= Flight1+X.

% check whether better than optimal solution exists
:- minimumcrew(N), #count{ Person : crew(Flight, Person) } >= N.

% symmetry breaking

diff(Person1, Person2) :- p(Person1, Type1), p(Person2, Type2), flight(Flight, Type1, Num), not p(Person2, Type1).
same(Person1, Person2) :- p(Person1, Type1), p(Person2, Type2), Person1 < Person2, not diff(Person1, Person2), not diff(Person2, Person1).

dist(Person1, Person2) :- same(Person1, Person), same(Person, Person2).
next(Person1, Person2) :- same(Person1, Person2), not dist(Person1, Person2).

init(Flight) :- #min{ F : num_crew(F, Num) } = Flight.
last(Flight) :- #max{ F : num_crew(F, Num) } = Flight.

symm(Person1, Person2, Flight)   :- next(Person1, Person2), init(Flight), num_crew(Flight, Num).
symm(Person1, Person2, Flight+1) :- symm(Person1, Person2, Flight), not last(Flight), crew(Flight, Person2).
symm(Person1, Person2, Flight+1) :- symm(Person1, Person2, Flight), not last(Flight), not crew(Flight, Person1).

:- symm(Person1, Person2, Flight), crew(Flight, Person2), not crew(Flight, Person1).
