# JavaScript Static Analysis

## JavasScript is Interpreter Language

Since JavaScript is a language with interpreter characteristics, the following operations are executed normally.

``` js
function x()
{
  console.log(b)
  b = -b
}

b = 1
x();
console.log(b);
x()
console.log(b);
```

Therefore, it cannot be analyzed in the same way as the compilation language.

## So how do you analyze this?

This is a very difficult problem. So I added some rules.

 - First, Check the IN set in block units.
 - Second, Check this OUT set in block units.

## IN, OUT set

What is IN, OUT set? IN set is just incoming value. 
This is mainly a function arguments in a compilation language.
In JavaScript, variables are stored dynamically, 
so it is impossible to determine the IN set from the function arguments alone.
OUT set is not a return value, but a union of a set of variables and an IN set that change in the function.
For example, consider the following function.

``` js
function x()
{
  console.log(b)
  b = -b
  c = 0
}
```

Function `x` is a function without arguments.
However, when analyzing the block of the function, the variable `b` and the `console` variable are used.
Therefore, the IN set contains `console` and `b` as elements.
The OUT set contains `console`, `b`, and `c` as elements.
When calculating the difference between IN and OUT(`OUT - IN`), `c` comes out, which means that this variable was created in function `x`.
Let's also look at the following example.

``` js
function y(a, b, c)
{
  b = -1
  c.a = 1;
  d = 1;
}
o = {a:2};
b = 1;
y(1,b,o);
console(b, o.a)
```

Since the function call parameter of JavaScript is passed as a copy of a reference, it is possible to modify the object inside the function, but it is not possible to change the object variable itself.

In the `y` function, the IN set contains only `c`, `d`.
The reason why a is not included is that it is not used anywhere in the function.
Also, `b` is not included, because the input value was modified before it was first used.
So what about the OUT set? Likewise, there are only `c` and `d` in the OUT set.
The reason `b` is missing is because the variable `b` is used only inside the function. Since `b` has already been declared as an argument to the function, it must operate with a different value from `b` outside of function.

### IN, OUT in nested block

To accurately calculate IN, OUT, a generalized method is needed.