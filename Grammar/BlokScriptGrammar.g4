grammar BlokScriptGrammar;

STATEMENTEND: ';';
WS: [ \r\t\n]+ -> skip ;
STRINGLITERAL: '\'' ([ a-zA-Z0-9%_/-] | '.' | '#')* '\'';
VARID: [a-zA-Z][a-zA-Z0-9_]+;
INTLITERAL: [0-9]+;
REGEXLITERAL: '/' ('^' | '$' | '*' | '[' | ']' | [a-z]+ | [A-Z]+ | [0-9]+ | '-' | '(' | ')' | '.' | '+' | '\\/')* '/';

script: statementList;

statementList: (statement STATEMENTEND)+;

statement: loginStatement
	| varStatement
	| copyStatement
	| assignmentStatement
	| printStatement
	| verbosityStatement
	| waitStatement
	| compareStatement
	| publishStoryStatement
	| unpublishStoryStatement
	| deleteStoryStatement
	| copyStoriesStatement
	| publishStoriesStatement
	| unpublishStoriesStatement
	| deleteStoriesStatement
	;

loginStatement: loginOnlyStatement 
	| loginWithGlobalUserNameStatement
	| loginWithGlobalPasswordStatement
	| loginWithGlobalTokenStatement
	| loginWithGlobalUserNameAndPasswordStatement
	;

loginOnlyStatement: 'login';
loginWithGlobalUserNameStatement: 'login' 'with' 'global' 'username';
loginWithGlobalPasswordStatement: 'login' 'with' 'global' 'password';
loginWithGlobalTokenStatement: 'login' 'with' 'global' 'token';
loginWithGlobalUserNameAndPasswordStatement: 'login' 'with' 'global' 'username' 'and' 'password';

varStatement: spaceVarStatement
	| blockVarStatement
	| stringVarStatement
	| regexVarStatement
	| storyVarStatement
	;

spaceVarStatement: 'space' VARID ('=' spaceSpec)?;
blockVarStatement: 'block' VARID ('=' blockSpec)?;
stringVarStatement: 'string' VARID ('=' (STRINGLITERAL | VARID | stringExpr))?;
regexVarStatement: 'regex' VARID ('=' (REGEXLITERAL | VARID))?;
storyVarStatement: 'story' VARID ('=' storySpec)?;

spaceSpec: 'space' (INTLITERAL | STRINGLITERAL | VARID) (varGetFrom)?
	| VARID
	;

blockSpec: 'block' STRINGLITERAL 'in' (spaceSpec | fileSpec)
	| 'block' VARID
	;

storySpec: (VARID | INTLITERAL | STRINGLITERAL) ('in' | 'from') (spaceSpec | fileSpec)
	| VARID
	;

assignmentStatement: VARID '=' VARID
	| spaceAssignmentStatement
	| stringAssignmentStatement
	| blockAssignmentStatement
	;

spaceAssignmentStatement: VARID '=' spaceSpec;
blockAssignmentStatement: VARID '=' blockSpec;
stringAssignmentStatement: VARID '=' STRINGLITERAL;

copyStatement: copyBlockStatement
	| copySpaceStatement
	| copySpacesStatement
	| copyBlocksStatement
	| copyStoryStatement
	;
	
copyBlockStatement: 'copy' blockSpec 'to' blockOutputLocation;
copySpaceStatement: 'copy' spaceSpec 'to' spaceOutputLocation;

copySpacesStatement: 'copy' 'all'? 'spaces' 'from' realDataLocation 'to' spacesOutputLocation;

printStatement: printSpacesStatement
	| printVarStatement
	| printSpaceStatement
	| printStringLiteralStatement
	| printSymbolTableStatement
	| printLocalCacheStatement
	;

printSpacesStatement: 'print' 'spaces' 'from' realDataLocation;
printVarStatement: 'print' VARID;
printSpaceStatement: 'print' 'space' (VARID | STRINGLITERAL);
printStringLiteralStatement: 'print' STRINGLITERAL;
printSymbolTableStatement: 'print' 'symbol' 'tables';
printLocalCacheStatement: 'print' 'local' 'cache';

realDataLocation: ('server' | 'local' 'cache');

spacesOutputLocation: 'console'
	| 'local' 'cache'
	| fileSpec
	;

fileSpec: 'file' (STRINGLITERAL | VARID)?;

blockOutputLocation: 'console'
	| 'local' 'cache'
	| 'file' STRINGLITERAL?
	| spaceSpec
	;

blocksOutputLocation: 'console'
	| 'local' 'cache'
	| fileSpec
	| filesSpec
	| spaceSpec
	;

storyOutputLocation: 'console'
	| 'local' 'cache'
	| fileSpec
	| spaceSpec
	;

filesSpec: 'files';

spaceOutputLocation: 'console'
	| 'local' 'cache'
	| fileSpec
	;

varGetFrom: ('on' 'demand' | 'in' 'local' 'cache' | 'on' 'server' | 'in' fileSpec);

copyBlocksStatement: 'copy' 'all'? 'blocks' ('where' blockConstraintList)? ('in' | 'from') spaceSpec 'to' blocksOutputLocation;

copyStoryStatement: 'copy' 'story' storySpec 'to' (storyOutputLocation | VARID);

blockConstraintList: blockConstraint ('and' blockConstraintList)?
	| blockConstraint ('or' blockConstraintList)?
	;
	
blockConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| 'name' ('=' | '!=') stringExpr
	| 'name' 'not'? 'in' '(' stringExprList ')'
	| 'name' ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| 'name' 'not'? 'in' '(' regexExprList ')'
	| 'name' 'not'? 'like' stringExpr
	| 'name' ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| 'name' ('ends' | 'does' 'not' 'end') 'with' stringExpr
	;

intExprList: intExpr (',' intExprList)?;

intExpr: (INTLITERAL | VARID) (('+' | '-' | '*' | '%') intExpr)?;

verbosityStatement: 'be'? ('verbose' | 'quiet');

waitStatement: 'wait' INTLITERAL;

compareStatement: compareSpacesStatement
	| compareBlocksStatement
	| compareAllBlocksStatement
	;

compareSpacesStatement: 'compare' spaceSpec 'and' spaceSpec;
compareBlocksStatement: 'compare' blockSpec 'and' blockSpec;
compareAllBlocksStatement: 'compare' 'all' 'blocks' 'in' spaceSpec 'and' spaceSpec;

publishStoryStatement: 'publish' 'story' storySpec ('in' spaceSpec)?;
unpublishStoryStatement: 'unpublish' 'story' storySpec ('in' spaceSpec)?;
deleteStoryStatement: 'delete' 'story' storySpec ('in' spaceSpec)?;

storiesInputLocation: 'local' 'cache'
	| fileSpec
	| filesSpec
	| spaceSpec
	;

storiesOutputLocation: 'console'
	| 'local' 'cache'
	| fileSpec
	| filesSpec
	| spaceSpec
	;

copyStoriesStatement: 'copy' 'stories' (('where' | 'with') storyConstraintList)? ('in' | 'from') storiesInputLocation 'to' storiesOutputLocation;

publishStoriesStatement: 'publish' 'stories' (('where' | 'with') storyConstraintList)? ('in' | 'from') spaceSpec;

unpublishStoriesStatement: 'unpublish' 'stories' (('where' | 'with') storyConstraintList)? ('in' | 'from') spaceSpec;

deleteStoriesStatement: 'delete' 'stories' (('where' | 'with') storyConstraintList)? ('in' | 'from') spaceSpec;

storyConstraintList: storyConstraint ('and' storyConstraintList)?
	| storyConstraint ('or' storyConstraintList)?
	;

storyConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| ('name' | 'url') ('=' | '!=') stringExpr
	| ('name' | 'url') 'not'? 'in' '(' stringExprList ')'
	| ('name' | 'url') ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| ('name' | 'url') 'not'? 'in' '(' stringExprList ')'
	| ('name' | 'url') 'not'? 'like' stringExpr
	| ('name' | 'url') ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| ('name' | 'url') ('ends' | 'does' 'not' 'end') 'with' stringExpr
	| (('any'? 'tag') | ('all'? 'tags')) ('=' | '!=') stringExpr
	| (('any'? 'tag') | ('all'? 'tags')) 'not'? 'in' '(' stringExprList ')'
	| 'any'? 'tag' ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| 'any'? 'tag' ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| 'any'? 'tag' ('ends' | 'does' 'not' 'end') 'with' stringExpr
	| 'all'? 'tags' ('match' | 'do' 'not' 'match') 'regex'? regexExpr
	| 'all'? 'tags' ('start' | 'do' 'not' 'start') 'with' stringExpr
	| 'all'? 'tags' ('end' | 'do' 'not' 'end') 'with' stringExpr
	| (('any'? 'tag') | ('all'? 'tags')) 'not'? 'in' '(' regexExprList ')'
	| (('any'? 'tag') | ('all'? 'tags')) 'not'? 'like' stringExpr
	| 'no' 'tags'
	| 'any' 'tags'
	;

regexExpr: STRINGLITERAL | REGEXLITERAL | VARID;

regexExprList: regexExpr (',' regexExprList)?;

copyDatasourceStatement: 'copy' 'datasource' (STRINGLITERAL | VARID | INTLITERAL) ('from' | 'in') (spaceSpec | VARID) 'to' (spaceSpec | VARID);

copyDatasourcesStatement: 'copy' 'datasources' datasourceConstraintList? ('from' | 'in') (spaceSpec | VARID) 'to' (spaceSpec | VARID);

datasourceConstraintList: datasourceConstraint (('and' | 'or') datasourceConstraintList)?;

datasourceConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| ('name' | 'slug') ('=' | '!=') stringExpr
	| ('name' | 'slug') 'not'? 'in' '(' stringExprList ')'
	| ('name' | 'slug') ('matches' | 'does' 'not' 'match') 'regex'? (stringExpr | REGEXLITERAL)
	| ('name' | 'slug') 'not'? 'in' '(' regexExprList ')'
	| ('name' | 'slug') 'not'? 'like' stringExpr
	| ('name' | 'slug') ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| ('name' | 'slug') ('ends' | 'does' 'not' 'end') 'with' (stringExpr)
	;

stringExpr: (STRINGLITERAL | VARID) ('+' stringExpr)?;

stringExprList: stringExpr (',' stringExprList)?;

forEachStatement: 'foreach' '(' ')' '{' statementList '}';
