<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use DB;

class Brand extends Model
{
    use HasFactory;
    protected $fillable = [
        'name',
        'description',
    ];
    protected $table = 'brands';
    public function getList(){
        $brands = brands::all();
        return $brands;
    }
    public function addBrand($data){
        return brands::insert($data);
    }
    public function getDetail($id){
        return DB::table($this->table)
        ->where('id',$id)
        ->first();
    }
    public function updateBrand($id,$data){
        return brands::where('id',$id)->update($data);
    }
    public function deleteItem($id){
        return brands::where('id',$id)->delete();
    }
    public function product(){
        return $this->hasMany(Product::class);
    }
}

