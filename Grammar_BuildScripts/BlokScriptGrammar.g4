grammar BlokScriptGrammar;

STATEMENTEND: ';';
WS: [ \r\t\n]+ -> skip ;
STRINGLITERAL: '\'' ([ a-zA-Z0-9%_/-] | '.' | '#')* '\'';
VARID: [a-zA-Z][a-zA-Z0-9_]+;
INTLITERAL: [0-9]+;
REGEXLITERAL: '/' ('^' | '$' | '*' | '[' | ']' | [a-z]+ | [A-Z]+ | [0-9]+ | '-' | '(' | ')' | '.' | '+' | '\\/')* '/';
LINE_COMMENT: '//' ~( '\n'|'\r' )* '\r'? '\n' -> skip;
BLOCK_COMMENT: '/*' .*? '*/' -> skip;

script: statementList;

statementList: (statement STATEMENTEND)+;

statement: loginStatement
	| varStatement
	| assignmentStatement
	| printStatement
	| verbosityStatement
	| waitStatement
	| compareStatement
	| createDatasourceEntryStatement
	| updateDatasourceEntriesStatement
	| deleteDatasourceEntriesStatement
	| copyDatasourceEntriesStatement
	| syncDatasourceEntriesStatement
	| copyBlocksStatement
	| copySpacesStatement
	| copyBlocksStatement
	| copyStoriesStatement
	| publishStoriesStatement
	| unpublishStoriesStatement
	| deleteStoriesStatement
	| copyDatasourcesStatement
	| deleteBlocksStatement
	| deleteDatasourcesStatement
	| createDatasourceStatement
	| updateDatasourceStatement
	| deleteDatasourceStatement
	;

createDatasourceStatement: 'create' 'datasource' (stringExpr | '(' datasourceUpdateList ')') ('for' | 'in') spaceSpec;
deleteDatasourceStatement: 'delete' 'datasource' (datasourceShortSpec | datasourceSpec);
updateDatasourceStatement: 'update' 'datasource' (datasourceShortSpec | datasourceSpec) 'set' datasourceUpdateList;

datasourceUpdateList: datasourceUpdate (',' datasourceUpdateList)?;

datasourceUpdate: 'name' '=' stringExpr
	| 'slug' '=' stringExpr
	;

createDatasourceEntryStatement: 'create' 'datasource' 'entry' (stringExpr | datasourceEntryUpdateList) ('for' | 'in') datasourceSpec;
updateDatasourceEntriesStatement: 'update' 'datasource' 'entries' 'in' datasourceSpec 'set' datasourceEntryUpdateList ('where' datasourceEntryConstraintExprList)?;
deleteDatasourceEntriesStatement: 'delete' 'datasource' 'entries' 'in' datasourceSpec ('where' datasourceEntryConstraintExprList)?;
copyDatasourceEntriesStatement: 'copy' 'datasource' 'entries' ('from' | 'in') datasourceEntriesSourceLocation 'to' datasourceEntriesTargetLocation ('where' datasourceEntryConstraintExprList)?;
syncDatasourceEntriesStatement: 'sync' 'datasource' 'entries' ('from' | 'in') datasourceEntriesSourceLocation 'to' datasourceEntriesSourceLocation ('where' datasourceEntryConstraintExprList)?;

datasourceEntryUpdateList: datasourceEntryUpdate (',' datasourceEntryUpdateList)?;

datasourceEntryUpdate: 'name' '=' stringExpr
	| 'value' '=' stringExpr
	;

datasourceEntriesSourceLocation: datasourceSpec
	| urlSpec
	| fileSpec
	| 'local cache'
	;

urlSpec: ('csv' | 'json')? 'url' stringExpr;

datasourceEntriesTargetLocation: datasourceSpec
	| urlSpec
	| fileSpec
	| 'local cache'
	| 'console'
	;
	
datasourceEntryConstraintExprList: datasourceEntryConstraintExpr (('and' | 'or') datasourceEntryConstraintExprList)?;

datasourceEntryConstraintExpr: datasourceEntryConstraint (('and' | 'or') datasourceEntryConstraintExpr)?
	| '(' datasourceEntryConstraint (('and' | 'or') datasourceEntryConstraintExpr)? ')'
	| '(' datasourceEntryConstraintExpr (('and' | 'or') datasourceEntryConstraintExpr)? ')'
	;

datasourceEntryConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| ('name' | 'value') ('=' | '!=') stringExpr
	| ('name' | 'value') 'not'? 'in' '(' (stringExprList | regexExprList) ')'
	| ('name' | 'value') ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| ('name' | 'value') 'not'? 'in' '(' stringExprList ')'
	| ('name' | 'value') 'not'? 'like' stringExpr
	| ('name' | 'value') ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| ('name' | 'value') ('ends' | 'does' 'not' 'end') 'with' stringExpr
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
	| datasourceEntryVarStatement
	| 'var' VARID '=' (VARID | spaceSpec | blockSpec | stringExpr | regexExpr | storySpec | intExpr | datasourceEntrySpec | datasourceSpec)
	;

spaceVarStatement: 'space' VARID ('=' spaceSpec)?;
blockVarStatement: 'block' VARID ('=' blockSpec)?;
stringVarStatement: 'string' VARID ('=' stringExpr)?;
regexVarStatement: 'regex' VARID ('=' regexExpr)?;
storyVarStatement: 'story' VARID ('=' storySpec)?;
datasourceEntryVarStatement: 'datasource' 'entry' VARID ('=' datasourceEntrySpec)?;

spaceSpec: 'space' (INTLITERAL | STRINGLITERAL | VARID) (varGetFrom)?
	| VARID
	;

shortSpaceSpec: INTLITERAL | STRINGLITERAL;

blockSpec: 'block' STRINGLITERAL 'in' (spaceSpec | fileSpec)
	| 'block' VARID
	;

storySpec: (VARID | INTLITERAL | STRINGLITERAL) ('in' | 'from') (spaceSpec | fileSpec)
	| VARID
	;

datasourceEntrySpec: 'datasource' 'entry' (intExpr | stringExpr | VARID) ('from' | 'in') datasourceSpec
	| VARID
	;

datasourceSpec: 'datasource' (VARID | INTLITERAL | STRINGLITERAL) 'in' spaceSpec
	| VARID
	;

datasourceShortSpec: (VARID | INTLITERAL | STRINGLITERAL) 'in' spaceSpec;

assignmentStatement: VARID '=' VARID
	| spaceAssignmentStatement
	| stringAssignmentStatement
	| blockAssignmentStatement
	;

spaceAssignmentStatement: VARID '=' spaceSpec;
blockAssignmentStatement: VARID '=' blockSpec;
stringAssignmentStatement: VARID '=' STRINGLITERAL;
	
copySpacesStatement: 'copy' 'spaces' 'from' realDataLocation 'to' spacesOutputLocation;

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

varGetFrom: ('on' 'server' | 'in' fileSpec);

createBlockStatement: 'create' 'block' '(' blockUpdateList ')' 'in' spaceSpec;
updateBlocksStatement: 'update' 'blocks' 'in' spaceSpec 'set' blockUpdateList ('where' blockConstraintExprList)?;
copyBlocksStatement: 'copy' 'blocks' ('in' | 'from') spaceSpec 'to' blocksOutputLocation ('where' blockConstraintExprList)?;
deleteBlocksStatement: 'delete' 'blocks' ('in' | 'from') spaceSpec ('where' blockConstraintExprList)?;

blockConstraintExprList: blockConstraintExpr (('and' | 'or') blockConstraintExprList)?;

blockConstraintExpr: blockConstraint (('and' | 'or') blockConstraintExpr)?
	| '(' blockConstraint (('and' | 'or') blockConstraintExpr)? ')'
	| '(' blockConstraintExpr (('and' | 'or') blockConstraintExpr)? ')'
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

blockUpdateList: blockUpdate (',' blockUpdateList)?;

blockUpdate: 'technical' 'name' '=' stringExpr
	| 'display' 'name' '=' stringExpr
	| 'type' '=' ('nestable' | 'content' | 'universal')
	| 'add' 'tag' stringExpr
	| 'remove' 'tag' stringExpr
	| 'preview' 'field' '=' stringExpr
	| 'preview' 'template' '=' stringExpr
	| 'preview' 'screenshot' '=' stringExpr
	;

intExprList: intExpr (',' intExprList)?;

intExpr: (INTLITERAL | VARID) (('+' | '-' | '*' | '%') intExpr)?;

verbosityStatement: 'be'? ('quiet' | 'verbose' | 'debugger');

waitStatement: 'wait' INTLITERAL;

compareStatement: compareSpacesStatement
	| compareBlocksStatement
	| compareAllBlocksStatement
	;

compareSpacesStatement: 'compare' spaceSpec 'and' spaceSpec;
compareBlocksStatement: 'compare' blockSpec 'and' blockSpec;
compareAllBlocksStatement: 'compare' 'all' 'blocks' 'in' spaceSpec 'and' spaceSpec;

storiesInputLocation: 'local' 'cache'
	| fileSpec
	| filesSpec
	| spaceSpec
	| shortSpaceSpec
	;

storiesOutputLocation: 'console'
	| 'local' 'cache'
	| fileSpec
	| filesSpec
	| spaceSpec
	| shortSpaceSpec
	;

copyStoriesStatement: 'copy' 'stories' ('in' | 'from') storiesInputLocation 'to' storiesOutputLocation ('where' storyConstraintExprList)?;
publishStoriesStatement: 'publish' 'stories' ('in' | 'from') spaceSpec ('where' storyConstraintExprList)?;
unpublishStoriesStatement: 'unpublish' 'stories' ('in' | 'from') spaceSpec ('where' storyConstraintExprList)?;
deleteStoriesStatement: 'delete' 'stories' ('in' | 'from') (spaceSpec | shortSpaceSpec) ('where' storyConstraintExprList)?;

storyConstraintExprList: storyConstraintExpr (('and' | 'or') storyConstraintExprList)?;

storyConstraintExpr: storyConstraint (('and' | 'or') storyConstraintExpr)?
	| '(' storyConstraint (('and' | 'or') storyConstraintExpr)? ')'
	| '(' storyConstraintExpr (('and' | 'or') storyConstraintExpr)? ')'
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

copyDatasourcesStatement: 'copy' 'datasources' ('from' | 'in') spaceSpec 'to' spaceSpec ('where' datasourceConstraintExprList)?;
updateDatasourcesStatement: 'update' 'datasources' ('from' | 'in') spaceSpec 'to' spaceSpec ('where' datasourceConstraintExprList)?;
deleteDatasourcesStatement: 'delete' 'datasources' ('from' | 'in') spaceSpec ('where' datasourceConstraintExprList)?;
syncDatasourcesStatement: 'copy' 'datasources' ('from' | 'in') spaceSpec 'to' spaceSpec ('where' datasourceConstraintExprList)?;

datasourceConstraintExprList: datasourceConstraintExpr (('and' | 'or') datasourceConstraintExprList)?;

datasourceConstraintExpr: datasourceConstraint (('and' | 'or') datasourceConstraintExpr)?
	| '(' datasourceConstraint (('and' | 'or') datasourceConstraintExpr)? ')'
	| '(' datasourceConstraintExpr (('and' | 'or') datasourceConstraintExpr)? ')'
	;

datasourceConstraint: 'id' ('=' | '!=') intExpr
	| 'id' 'not'? 'in' '(' intExprList ')'
	| ('name' | 'slug') ('=' | '!=') stringExpr
	| ('name' | 'slug') 'not'? 'in' '(' stringExprList ')'
	| ('name' | 'slug') ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
	| ('name' | 'slug') 'not'? 'in' '(' regexExprList ')'
	| ('name' | 'slug') 'not'? 'like' stringExpr
	| ('name' | 'slug') ('starts' | 'does' 'not' 'start') 'with' stringExpr
	| ('name' | 'slug') ('ends' | 'does' 'not' 'end') 'with' (stringExpr)
	;

stringExprList: stringExpr (',' stringExprList)?;

stringExpr: (STRINGLITERAL | VARID) ('+' stringExpr)?;

forEachStatement: 'foreach' '(' ')' '{' statementList '}';
